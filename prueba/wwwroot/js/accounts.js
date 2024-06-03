var app = new Vue({
    el:"#app",
    data:{
        clientInfo: {},
        //error: null
        errorToats: null,
        errorMsg: null,
    },
    methods:{
        getData: function(){
            //axios.get("/api/clients/1")
            axios.get("/api/clients/current")
            .then(function (response) {
                //get client ifo
                app.clientInfo = response.data;
                console.log(response)
            })
            .catch(function (error) {
                // handle error
                //app.error = error;
                this.errorMsg = "Error getting data";
                this.errorToats.show();
            })
        },
        formatDate: function(date){
            return new Date(date).toLocaleDateString('en-gb');
        },
        signOut: function () {
            axios.post('/api/auth/logout')
                .then(response => window.location.href = "/index.html")
                .catch(() => {
                    this.errorMsg = "Sign out failed"
                    this.errorToats.show();
                })
        },
    },
    mounted: function () {
        this.errorToats = new bootstrap.Toast(document.getElementById('danger-toast'));
        this.getData();
    }
})