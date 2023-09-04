import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {NavbarComponent} from './navbar/navbar.component';
import {HomeComponent} from './home/home.component';
import {FooterComponent} from './footer/footer.component';
import {LoginComponent} from './auth-components/login/login.component';
import {RegisterComponent} from './auth-components/register-component/register.component';
import {AuthInterceptor} from "./services/auth.interceptor";

import {ToastrModule, GlobalConfig} from "ngx-toastr";
import {UserProfileComponent} from './profile-components/user-profile/user-profile.component';
import {ChatComponent} from './messanger-components/chat/chat.component';
import {JWT_OPTIONS, JwtHelperService, JwtModule, JwtModuleOptions} from "@auth0/angular-jwt";
import {SignalRMessageService} from "./services/signalR/signal-r-message.service";
import {ChatDetailsComponent} from './messanger-components/chat-details/chat-details.component';
import {ChatDomainComponent} from './messanger-components/chat-domain/chat-domain.component';
import {TruncateTextPipe} from './services/pipes/truncate-text.pipe';
import {EditChatModalComponent} from './messanger-components/edit-chat-modal/edit-chat-modal.component';
import {BrowserAnimationsModule, provideAnimations} from '@angular/platform-browser/animations';

export function jwtOptionsFactory(): JwtModuleOptions {
    return {
        // @ts-ignore
        allowedDomains: ['example.com'], // Разрешенные домены для отправки токена
        disallowedRoutes: ['example.com/api/auth'], // Маршруты, которым запрещено отправлять токен
    };
}

@NgModule({
    declarations: [
        AppComponent,
        NavbarComponent,
        HomeComponent,
        FooterComponent,
        LoginComponent,
        RegisterComponent,
        UserProfileComponent,
        ChatComponent,
        ChatDetailsComponent,
        ChatDomainComponent,
        TruncateTextPipe,
        EditChatModalComponent,
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        FormsModule,
        RoutingModule,
        ToastrModule.forRoot({
            timeOut: 4000,
            progressBar: true,
            positionClass: 'toast-bottom-right',
        } as GlobalConfig),
        ReactiveFormsModule,
        JwtModule.forRoot({
            jwtOptionsProvider: {
                provide: JWT_OPTIONS,
                useFactory: jwtOptionsFactory
            }
        }),
        BrowserAnimationsModule,
    ],
    providers: [{
        provide: HTTP_INTERCEPTORS,
        useClass: AuthInterceptor,
        multi: true,
    }, SignalRMessageService, JwtHelperService, provideAnimations()],
    bootstrap: [AppComponent]
})
export class AppModule {
}
