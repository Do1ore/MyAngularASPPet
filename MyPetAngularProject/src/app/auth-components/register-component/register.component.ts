import { Component } from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  email!: string;
  password!: string;
  repeatedPassword!: string;

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    const data = { email: this.email, password: this.password};

    if(this.repeatedPassword != data.password)
    {
      return;
    }
    this.authService.register(data).subscribe(() => {
      this.router.navigate(['/login']);
    });
  }
}
