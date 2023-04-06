import {Component, OnInit} from '@angular/core';
import {DarkmodeService} from "../services/darkmode.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  constructor(private darkMode: DarkmodeService) {
  }

  setUpDarkMode(): void {
    this.darkMode.setUpDarkMode()
  }

  setUpLightMode(): void {
    this.darkMode.setUpLightMode()
  }

  ngOnInit(): void {
    this.darkMode.setUpThemes();
  }
}
