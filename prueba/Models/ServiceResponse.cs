﻿namespace HomeBanking.Models {
    public class ServiceResponse<T> { //Patron Result a medias
        public T objectResponse;
        public int status;
        public string message;

        public ServiceResponse(T objectResponse, int status, string message) {
            this.objectResponse = objectResponse;
            this.status = status;
            this.message = message;
        }
    }
}
