import { HttpClient } from "@angular/common/http";
import { computed, inject, Injectable, signal } from "@angular/core";
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
    private readonly tokenState = signal<string | null>(this.readStoredToken());
    private readonly userState = signal<LoginResponse | null>(this.readStoredUser());

    readonly currentUser = this.userState.asReadonly();

    readonly isAuthenticated = computed(() => this.tokenState());
    readonly userRole = computed(() => this.userState()?.role ?? null);

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
                        );
                        this.tokenState.set(response.token);
                        this.userState.set(response);
                    }
                )
            )
    }

    logout(): void {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        this.tokenState.set(null);
        this.userState.set(null);
    }

    getToken(): string | null {
        return localStorage.getItem('token');
    }

    private readStoredUser(): LoginResponse | null {
        if (typeof localStorage === 'undefined') {
            return null;
        }

        const storedUser = localStorage.getItem('user');

        if (!storedUser) {
            return null;
        }

        try {
            return JSON.parse(storedUser) as LoginResponse;
        } catch (error) {
            return null;
        }
    }

    private readStoredToken(): string | null {
        if (typeof localStorage === 'undefined') {
            return null;
        }

        return localStorage.getItem('token');
    }


}