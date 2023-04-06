import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class DarkmodeService {
  setUpThemes(): void {
    if (localStorage['theme'] === 'dark' || (!('theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
      document.documentElement.classList.add('dark')
    } else {
      document.documentElement.classList.remove('light')
    }

// Whenever the user explicitly chooses to respect the OS preference
    localStorage.removeItem('theme')
  }

  setUpLightMode(): void {
    // Whenever the user explicitly chooses light mode
    localStorage['theme'] = 'light'
  }

  setUpDarkMode(): void {
    // Whenever the user explicitly chooses dark mode
    localStorage['theme'] = 'dark'
  }

}

