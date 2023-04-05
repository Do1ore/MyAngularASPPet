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

  constructor(private authService: AuthService, private router: Router) {
  }

  onSubmit(user: User, repeatedPassword: string): void {
    if (repeatedPassword != user.password) {
      alert("Incorrect password!😁")
      return;
    }
    this.authService.register(user).subscribe(() => {
      this.router.navigate(['/login']);
    });
  }

}
