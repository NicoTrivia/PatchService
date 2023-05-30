import { NgModule, LOCALE_ID } from '@angular/core';
import localeFr from '@angular/common/locales/fr';
import { APP_BASE_HREF, CommonModule, registerLocaleData } from '@angular/common';
import {HttpClientModule, HttpClient, HTTP_INTERCEPTORS} from '@angular/common/http';
import { FormsModule } from '@angular/forms';

registerLocaleData(localeFr);
import { BrowserModule } from '@angular/platform-browser';
// translate
import {TranslateModule, TranslateLoader} from '@ngx-translate/core';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';
// LOG
import { LoggerModule, NgxLoggerLevel } from 'ngx-logger';

import { AppRoutingModule } from './app-routing.module';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';


// UI
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { MessageModule } from 'primeng/message';
import { MessagesModule } from 'primeng/messages';

import {DialogModule} from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { InputSwitchModule } from 'primeng/inputswitch';

import { AppComponent } from './app.component';
import { RequestPatchComponent } from './nav/request-patch/request-patch.component';
import { SidebarComponent } from './nav/sidebar/sidebar.component';

// services
import { AuthenticationService } from './auth/authentication-service/authentication-service';
import { LoginComponent } from './auth/login/login.component';
import { LogoutComponent } from './auth/logout/logout.component';
import { TawkService} from './services/TawkService';

// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/');
}

@NgModule({
  declarations: [
    AppComponent,
    RequestPatchComponent,
    SidebarComponent,
    LoginComponent,
    LogoutComponent
  ],
  imports: [
    BrowserModule, AppRoutingModule,  CommonModule,HttpClientModule, BrowserAnimationsModule, FormsModule,
    FileUploadModule, DialogModule, ToastModule, DropdownModule, InputSwitchModule,MessageModule, MessagesModule,
    LoggerModule.forRoot({
      serverLoggingUrl: '/api/logs',
      level: NgxLoggerLevel.INFO,
      serverLogLevel: NgxLoggerLevel.ERROR
    }),
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
      },
      defaultLanguage: 'en'
    })
  ],
  providers: [
    AuthenticationService,
    TawkService,
    {provide: APP_BASE_HREF, useValue : '/front/' },
    { provide: LOCALE_ID, useValue: navigator.language}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
