var app = new Vue({
    el:"#app",
    data:{
        errorToats: null,
        errorMsg: null,
        cardType:"none",
        cardColor:"none",
    },
    methods:{
        formatDate: function(date){
            return new Date(date).toLocaleDateString('en-gb');
        },
        signOut: function(){
            axios.post('/api/auth/logout')
            .then(response => window.location.href="/index.html")
            .catch(() =>{
                this.errorMsg = "Sign out failed"   
                this.errorToats.show();
            })
        },
        create: function(event){
            event.preventDefault();
            if(this.cardType == "none" || this.cardColor == "none"){
                this.errorMsg = "You must select a card type and color";  
                this.errorToats.show();
            }else{
                console.log(this.cardColor)
                console.log(this.cardType)
/*                 let config = {
                    headers: {
                        'content-type': 'application/x-www-form-urlencoded'
                    }
                }
                axios.post(`http://localhost:8080/api/clients/current/cards?cardType=${this.cardType}&cardColor=${this.cardColor}`,config)
                .then(response => window.location.href = "/web/cards.html")
                .catch((error) =>{
                    this.errorMsg = error.response.data;  
                    this.errorToats.show();
                }) */
                axios.post('/api/clients/current/cards',{
                    type: this.cardType,
                    color: this.cardColor
                })
                    .then(() => { window.location.href = "/cards.html" })
                    .catch(error => {
                        console.log(error)
                        this.errorMsg = error.response.data
                        this.errorToats.show();
                    })
            }
        }
    },
    mounted: function(){
        this.errorToats = new bootstrap.Toast(document.getElementById('danger-toast'));
    }
})