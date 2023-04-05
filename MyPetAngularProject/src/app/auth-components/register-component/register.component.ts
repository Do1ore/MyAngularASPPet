import {Component} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {User} from "../../models/user";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  user = new User();
  repeatedPassword!: string;
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {
  }

  onSubmit(user: User, repeatedPassword: string): void {
    if (repeatedPassword != user.password) {
      this.errorMessage = "Incorrect repeated password"
      return;
    }
    else if(user.email.length === 0 || user.username.length === 0 || user.password.length === 0)
    {
      this.errorMessage = "Some inputs are empty"
      return;
    }
    this.authService.register(user).subscribe(() => {
        this.router.navigate(['/login']);
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
