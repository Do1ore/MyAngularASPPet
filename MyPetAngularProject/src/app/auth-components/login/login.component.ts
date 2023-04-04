import { Component } from '@angular/core';
import {Router} from "@angular/router";
import {AuthService} from "../../services/auth.service";


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  email!: string;
  password!: string;

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    const data = { email: this.email, password: this.password };
    this.authService.login(data).subscribe(() => {
      this.router.navigate(['/home']);
    });
  }
}
