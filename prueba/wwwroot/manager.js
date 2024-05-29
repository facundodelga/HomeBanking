const app = new Vue({
    el: '#app',
    data: {
        email: "",
        firstName: "",
        lastName: "",
        outPut: "",
        clients: []
    },
    methods:{
        // load and display JSON sent by server for /clients
        loadData: function() {
            axios.get("/api/clients")
            .then(function (response) {
                // handle success
                app.outPut = response.data;
                app.clients = response.data;
            })
            .catch(function (error) {
                // handle error
                app.outPut = error;
            })
        },
         // handler for when user clicks add client
        addClient: function() {
            if (app.email.length > 1 && app.firstName.length > 1 && app.lastName.length > 1) {
                this.postPlayer(app.email,app.firstName,app.lastName);
            }
        },
        // code to post a new player using AJAX
        // on success, reload and display the updated data from the server
        postPlayer: function(email, firstName, lastName) {
             axios.post("clients",{ "email":email, "firstName": firstName, "lastName": lastName })
            .then(function (response) {
                // handle success
                showOutput = "Saved -- reloading";
                app.loadData();
                app.clearData();
            })
            .catch(function (error) {
                // handle error
                app.outPut = error;
            })
        },
        clearData: function(){
            app.firstName = "";
            app.lastName = "";
            app.email = "";
        }
    },
    mounted(){
        this.loadData();
    }
});

