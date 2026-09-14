import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { empty, Observable, of, tap } from "rxjs";
import { json } from "stream/consumers";

interface LoginRequest {
    email: string;
    password: string;
}

interface LoginResponse {
    token: string;
    userId: string;
    email: string;
    role: string;

}

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private http = inject(HttpClient);
    private readonly apiUrl = 'http://localhost:8000/api';

    login(request: LoginRequest): Observable<LoginResponse> {
        return this.http
            .post<LoginResponse>(`${this.apiUrl}/auth/login`, request)
            .pipe(
                tap(
                    response => {
                        localStorage.setItem(
                            'token',
                            response.token
                        );
                        localStorage.setItem(
                            'user',
                            JSON.stringify(response)
                        )
                    }
                )
            )
    }

    logout() : void{
        localStorage.removeItem('token');
        localStorage.removeItem('user');
    }

    getToken () : string | null{
        return localStorage.getItem('token');   
    }

    isAuthenticated() : boolean{
        return !!!this.getToken();
    }

}