var app = new Vue({
    el:"#app",
    data:{
        loanTypes: [],
        loanTypeId: 0,
        payments: 0,
        paymentsList: [],
        clientAccounts: [],
        errorToats: null,
        errorMsg: null,
        accountToNumber: "VIN",
        amount: 0,
        fees: []
    },
    methods:{
        getData: function(){
            Promise.all([axios.get("/api/loans"),axios.get("/api/clients/current/accounts")])
            .then((response) => {
                //get loan types ifo
                this.loanTypes = response[0].data;
                this.clientAccounts = response[1].data;
            })
            .catch((error) => {
                this.errorMsg = "Error getting data";
                this.errorToats.show();
            })
        },
        formatDate: function(date){
            return new Date(date).toLocaleDateString('en-gb');
        },
        checkApplication: function(){
            if(this.loanTypeId == 0){
                this.errorMsg = "You must select a loan type";
                this.errorToats.show();
            }
            else if(this.payments == 0){
                this.errorMsg = "You must select payments";
                this.errorToats.show();
            }
            else if(this.accountToNumber == "VIN"){
                this.errorMsg = "You must indicate an account";
                this.errorToats.show();
            }
            else if(this.amount == 0){
                this.errorMsg = "You must indicate an amount";  
                this.errorToats.show();
            }else{
                this.modal.show();
            }
        },
        apply: function(){
            axios.post("/api/loans",{loanId: this.loanTypeId, amount: this.amount, payments: this.payments, toAccountNumber: this.accountToNumber})
            .then(response => { 
                this.modal.hide();
                this.okmodal.show();
            })
            .catch((error) =>{
                this.errorMsg = error.response.data;  
                this.errorToats.show();
            })
        },
        changedType: function(){
            this.paymentsList = this.loanTypes.find(loanType => loanType.id == this.loanTypeId).payments.split(',');
        },
        finish: function(){
            window.location.reload();
        },
        checkFees: function(){
            this.fees = [];
            this.totalLoan = parseInt(this.amount) + (this.amount * 0.2);
            let amount = this.totalLoan / this.payments;
            for(let i = 1; i <= this.payments; i++){
                this.fees.push({ amount: amount });
            }
            this.feesmodal.show();
        },
        signOut: function(){
            axios.post('/api/auth/logout')
            .then(response => window.location.href="/index.html")
            .catch(() =>{
                this.errorMsg = "Sign out failed"   
                this.errorToats.show();
            })
        },
    },
    mounted: function(){
        this.errorToats = new bootstrap.Toast(document.getElementById('danger-toast'));
        this.modal = new bootstrap.Modal(document.getElementById('confirModal'));
        this.okmodal = new bootstrap.Modal(document.getElementById('okModal'));
        this.feesmodal = new bootstrap.Modal(document.getElementById('feesModal'));
        this.getData();
    }
})