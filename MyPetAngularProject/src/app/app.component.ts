import {Component, OnInit} from '@angular/core';
import {SignalRConnectionService} from "./services/signalR/signalr-connection.service";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    title = 'MyPetAngularProject';

    constructor(private connectionService: SignalRConnectionService) {
    }

    ngOnInit(): void {

    }
}
