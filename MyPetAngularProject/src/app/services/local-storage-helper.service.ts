import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({
    providedIn: 'root'
})
export class LocalStorageHelperService {
    private authTokenName = environment.authTokenName;

    constructor(private readonly jwtHelper: JwtHelperService) {
    }

    public getUserIdFromToken(): string {
        const token = localStorage.getItem(this.authTokenName);
        if (token === null) {
            return '';
        }
        const decodedToken = this.jwtHelper.decodeToken(token);
        return decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    }
}
