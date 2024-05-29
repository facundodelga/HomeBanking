var app = new Vue({
    el:"#app",
    data:{
        accountInfo: {},
        error: null
    },
    methods:{
        getData: function(){
            const urlParams = new URLSearchParams(window.location.search);
            const id = urlParams.get('id');
            axios.get(`/api/accounts/${id}`)
            .then(function (response) {
                //get client ifo
                app.accountInfo = response.data;
                app.accountInfo.transactions.sort((a,b) => parseInt(b.id - a.id))
            })
            .catch(function (error) {
                // handle error
                app.error = error;
            })
        },
        formatDate: function(date){
            return new Date(date).toLocaleDateString('en-gb');
        }
    },
    mounted: function(){
        this.getData();
    }
})