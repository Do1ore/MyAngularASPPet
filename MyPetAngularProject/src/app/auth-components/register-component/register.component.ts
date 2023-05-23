import {Component} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {UserFull} from "../../models/fullUser";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  user = new UserFull();
  repeatedPassword!: string;
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router, private toastService: ToastrService) {
  }

  onSubmit(user: UserFull, repeatedPassword: string): void {
    if (repeatedPassword != user.password) {
      this.errorMessage = "Incorrect repeated password"
      this.toastService.error(this.errorMessage + '😒', 'Error');
      return;
    } else if (user.email.length === 0 || user.password.length === 0) {
      this.errorMessage = "Some inputs are empty"
      this.toastService.error(this.errorMessage + '😒', 'Error');

      return;
    }
    this.authService.register(user).subscribe(() => {
        this.router.navigate(['/login']);
        this.toastService.success('Account registered👍', 'Success');
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
            this.errorMessage = 'Unknown error'
            break;
        }
        this.toastService.error(this.errorMessage + '😒', 'Error');
      });
  }

}
