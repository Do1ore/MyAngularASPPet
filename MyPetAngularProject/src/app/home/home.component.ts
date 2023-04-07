import {Component, OnInit} from '@angular/core';
import {DarkmodeService} from "../services/darkmode.service";
import {error} from "@angular/compiler-cli/src/transformers/util";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  constructor(private darkMode: DarkmodeService) {
  }

  ngOnInit(): void {
    this.darkMode.setUpThemes();
  }
}
