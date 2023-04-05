import {Component} from '@angular/core';
import {Router} from "@angular/router";
import {AuthService} from "../../services/auth.service";
import {User} from "../../models/user";


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  user = new User();
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {
  }

  onSubmit(user: User): void {
    if (user.email.length === 0) {
      user.email = user.username;
    } else {
      user.username = user.email;
    }

    this.authService.login(user).subscribe((token: string) => {
        this.errorMessage = '';
        localStorage.setItem('authToken', token)
        this.router.navigate(['']);
      },
      error => {
        console.log(`error status : ${error.status} ${error.message}`);
        switch (error.status) {
          case 400:
            this.errorMessage = 'Invalid input data'
            break;
          case 403:     //forbidden
            this.errorMessage = 'Error 403'
            break;
          default:
            this.errorMessage = "Input error"
            break;
        }
      });
  }
}
