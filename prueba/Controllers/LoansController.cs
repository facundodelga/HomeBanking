﻿using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repository;
using HomeBanking.Repository.Implementations;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeBanking.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase {
        
        private IClientLoanService _loanService;
        public LoansController(IClientLoanService clientLoanService)
        {
            _loanService = clientLoanService;
        }

        [HttpGet]
        
        public IActionResult Get() {
            try {
                var loans = loanRepository.GetAllLoans();

                var loansDTO = new List<LoanDTO>();

                foreach (var loan in loans) {
                    var loandto = new LoanDTO(loan);

                    loansDTO.Add(loandto);
                }

                return Ok(loansDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Get(long id) {
            try {
                var loan = loanRepository.FindById(id);
                if (loan == null) {
                    return Forbid();
                }
                var loanDTO = new LoanDTO(loan);
                return Ok(loanDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "ClientOnly")]
        public IActionResult PostLoan([FromBody] LoanApplicationDTO loanDTO) {
            try {
                //Que el usuario este autentificado
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) {
                    return StatusCode(403, "Forbidden");
                }
                
                var loanResponse = _loanService.MakeLoan(loanDTO,email);
                if (loanResponse.cl == null)
                    return StatusCode(loanResponse.status, "Data not valid");

                var clDTO = new ClientLoanDTO(loanResponse.cl);

                return StatusCode(loanResponse.status,clDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}
