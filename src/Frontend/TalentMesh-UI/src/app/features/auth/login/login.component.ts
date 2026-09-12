import { Component, inject } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { AuthService } from "../../../core/services/auth.service";
import { Router } from "@angular/router";
import { email } from "@angular/forms/signals";

@Component({
    selector : 'login',
    standalone : true,
    imports : [FormsModule],
    templateUrl : 'login.component.html'
})
export class LoginComponent{
    private autService = inject(AuthService);
    private router = inject(Router);

     email = '';
     password = '';

    login() : void{
        this.autService.login({
            email : this.email,
            password : this.password
        })
        .subscribe({
            next : () => {
                this.router.navigate(['/']);
            },
            error : (err) => {
                console.log(err);
            }
        })
    }

}   