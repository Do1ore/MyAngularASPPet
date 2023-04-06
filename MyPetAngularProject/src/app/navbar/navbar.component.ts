import {Component, ElementRef, ViewChild} from '@angular/core';
import {AuthService} from "../services/auth.service";
import {Observable} from "rxjs";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {

  set isAuthorized(value: boolean) {
    this._isAuthorized = value;
  }
  userName: string = '';
  public _isAuthorized: boolean = true;


  constructor(private authService: AuthService) {
    this.authService.getMe().subscribe((name: string) => {
        this.userName = name;
      },
      error => {
        this._isAuthorized = false;
      });
  }

  logout(): void {
    this.authService.logOut();
    this._isAuthorized = false;
  }
}
