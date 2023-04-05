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

  constructor(private authService: AuthService, private router: Router) {
  }

  onSubmit(user: User): void {
    if (user.email.length === 0) {
      user.email = user.username;
    } else {
      user.username = user.email;
    }
    this.authService.login(user).subscribe((token: string) => {
      localStorage.setItem('authToken', token),
      this.router.navigate(['']);
    });
  }

}
