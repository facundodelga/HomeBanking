var app = new Vue({
    el:"#app",
    data:{
        clientAccounts: [],
        clientAccountsTo: [],
        debitCards: [],
        errorToats: null,
        errorMsg: null,
        accountFromNumber: "VIN",
        accountToNumber: "VIN",
        trasnferType: "own",
        amount: 0,
        description: ""
    },
    methods:{
        getData: function(){
            axios.get("/api/clients/current/accounts")
            .then((response) => {
                //get client ifo
                this.clientAccounts = response.data;
            })
            .catch((error) => {
                console.log(error)
                this.errorMsg = "Error getting data";
                this.errorToats.show();
            })
        },
        formatDate: function(date){
            return new Date(date).toLocaleDateString('en-gb');
        },
        checkTransfer: function(){
            if(this.accountFromNumber == "VIN"){
                this.errorMsg = "You must select an origin account";  
                this.errorToats.show();
            }
            else if(this.accountToNumber == "VIN"){
                this.errorMsg = "You must select a destination account";  
                this.errorToats.show();
            }else if(this.amount == 0){
                this.errorMsg = "You must indicate an amount";  
                this.errorToats.show();
            }
            else if(this.description.length <= 0){
                this.errorMsg = "You must indicate a description";  
                this.errorToats.show();
            }else{
                this.modal.show();
            }
        },
        transfer: function(){
/*             let config = {
                headers: {
                    'content-type': 'application/x-www-form-urlencoded'
                }
            }
            axios.post(`/api/transactions?
            fromAccountNumber=${this.accountFromNumber}&toAccountNumber=${this.accountToNumber}&amount=${this.amount}&description=${this.description}`,config) */
            axios.post('/api/transactions',
                {
                    fromAccountNumber: this.accountFromNumber,
                    toAccountNumber: this.accountToNumber,
                    amount: this.amount,
                    description: this.description
                }
            )
            .then(response => { 
                this.modal.hide();
                this.okmodal.show();
            })
            .catch((error) =>{
                console.log(error.response.data)
                this.errorMsg = error.response.data;  
                this.errorToats.show();
            })
        },
        changedType: function(){
            this.accountFromNumber = "VIN";
            this.accountToNumber = "VIN";
        },
        changedFrom: function(){
            if(this.trasnferType == "own"){
                this.clientAccountsTo = this.clientAccounts.filter(account => account.number != this.accountFromNumber);
                this.accountToNumber = "VIN";
            }
        },
        finish: function(){
            window.location.reload();
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
        this.getData();
    }
})