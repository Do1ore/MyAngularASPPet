import {Component, OnInit} from '@angular/core';
import {AuthService} from "../services/auth.service";
import {DarkmodeService} from "../services/darkmode.service";
import {Subject} from "rxjs";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  private logoutSubject = new Subject<void>();
  public logout$ = this.logoutSubject.asObservable();

  set isAuthorized(value: boolean) {
    this._isAuthorized = value;
  }

  userName: string = '';
  public _isAuthorized: boolean = true;


  constructor(private authService: AuthService, private themeService: DarkmodeService) {
  }

  ngOnInit(): void {
    this.authService.userEmail$.subscribe((email: string) => {
      this.userName = email;
    });
    this.authService.getMe().subscribe((name: string) => {
        this.userName = name;
      },
      error => {
        this._isAuthorized = false;
      });
    this.setUpExpandNavbar();
    this.themeService.setUpThemes();
    this._isAuthorized = this.authService.isAuthorized();
  }

  logout(): void {
    this.authService.logOut();
    this.isAuthorized = false;
  }

  onClick(): void {
    let themeToggleDarkIcon = document.getElementById('theme-toggle-dark-icon');
    let themeToggleLightIcon = document.getElementById('theme-toggle-light-icon');
// Change the icons inside the button based on previous settings
    let themeToggleBtn = document.getElementById('theme-toggle');

    // toggle icons inside button
    themeToggleDarkIcon?.classList.toggle('hidden');
    themeToggleLightIcon?.classList.toggle('hidden');

    // if set via local storage previously
    if (localStorage.getItem('color-theme')) {
      if (localStorage.getItem('color-theme') === 'light') {
        document.documentElement.classList.add('dark');
        localStorage.setItem('color-theme', 'dark');
      } else {
        document.documentElement.classList.remove('dark');
        localStorage.setItem('color-theme', 'light');
      }
      // if NOT set via local storage previously
    } else {
      if (document.documentElement.classList.contains('dark')) {
        document.documentElement.classList.remove('dark');
        localStorage.setItem('color-theme', 'light');
      } else {
        document.documentElement.classList.add('dark');
        localStorage.setItem('color-theme', 'dark');
      }
    }
  }

  setUpExpandNavbar(): void {
    const toggleMenu = () => {
      const menu = document.getElementById('navbar-search');
      if (menu?.classList.contains('hidden')) {
        menu.classList.remove('hidden');
      } else {
        menu?.classList.add('hidden');
      }
    };

    const menuButton = document.querySelector('[data-collapse-toggle="navbar-search"]');
    menuButton?.addEventListener('click', toggleMenu);

    const mainMenuButton = document.getElementById('expand-menu-button');
    mainMenuButton?.addEventListener('click', toggleMenu);
  }
}
