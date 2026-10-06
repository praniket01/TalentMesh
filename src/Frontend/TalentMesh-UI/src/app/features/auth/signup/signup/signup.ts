import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { email } from '@angular/forms/signals';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './signup.html',
  styleUrl: './signup.css',
})
export class Signup {
  private readonly fb = new FormBuilder();
  signupForm = this.fb.nonNullable.group({
    name: ['', Validators.required],
    email: ['',
      Validators.required,
      Validators.email
    ],
    password: ['', [
      Validators.required,
      Validators.minLength(8)
    ]],
    confirmPassword: ['', Validators.required]
  });

  submit() : void {
    if (this.signupForm.invalid) {
      this.signupForm.markAllAsTouched();
      return;
    }
    console.log(
      'Signup form:',
      this.signupForm.getRawValue()
    );
  }

}
