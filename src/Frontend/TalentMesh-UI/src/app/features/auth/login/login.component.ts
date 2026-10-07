import { Component, inject } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AuthService } from "../../../core/services/auth.service";
import { Router } from "@angular/router";

@Component({
    selector: 'login',
    standalone: true,
    imports: [FormsModule, ReactiveFormsModule],
    templateUrl: 'login.component.html',
    styleUrl: 'login.component.css',
})
export class LoginComponent {
    private autService = inject(AuthService);
    private router = inject(Router);

    email = '';
    password = '';
    isLoading = false;
    errorMessage = '';


    login(): void {

        if (!this.email || this.password) {
            this.errorMessage = 'Please enter your Email or Password';
        }

        this.isLoading = true;
        this.errorMessage = '';


        this.autService.login({
            email: this.email,
            password: this.password
        })
            .subscribe({
                next: () => {
                    this.isLoading = false;
                    this.router.navigate(['/dashboard']);
                },
                error: (err) => {
                    console.log(err);
                    this.isLoading = false;
                    this.errorMessage =
                        err?.error?.message ??
                        'Invalid email or password. Please try again.';
                }
            })
    }

}   