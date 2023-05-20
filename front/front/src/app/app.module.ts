import { NgModule, LOCALE_ID } from '@angular/core';
import localeFr from '@angular/common/locales/fr';
import { APP_BASE_HREF, CommonModule, registerLocaleData } from '@angular/common';
import {HttpClientModule, HttpClient, HTTP_INTERCEPTORS} from '@angular/common/http';

registerLocaleData(localeFr);
import { BrowserModule } from '@angular/platform-browser';
// translate
import {TranslateModule, TranslateLoader} from '@ngx-translate/core';
import {TranslateHttpLoader} from '@ngx-translate/http-loader';

import { AppRoutingModule } from './app-routing.module';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';


// UI
import { FileUploadModule } from 'primeng/fileupload';
import { MessageService } from 'primeng/api';
import {DialogModule} from 'primeng/dialog';

import { AppComponent } from './app.component';
import { RequestPatchComponent } from './nav/request-patch/request-patch.component';
import { SidebarComponent } from './nav/sidebar/sidebar.component';


// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/');
}

@NgModule({
  declarations: [
    AppComponent,
    RequestPatchComponent,
    SidebarComponent
  ],
  imports: [
    BrowserModule, AppRoutingModule,  CommonModule,HttpClientModule, BrowserAnimationsModule,
    FileUploadModule, DialogModule,

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
    MessageService,
    {provide: APP_BASE_HREF, useValue : '/front/' },
    { provide: LOCALE_ID, useValue: navigator.language}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
