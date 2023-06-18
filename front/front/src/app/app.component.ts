import { Component } from '@angular/core';
import {Config} from './config';
import { TranslateService } from '@ngx-translate/core';
import { Pipe, PipeTransform} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Patch Services';
  constructor(private readonly translate: TranslateService) {
    this.initBaseUrl();
    this.title = Config.APP_TITLE;

    // this language will be used as a fallback when a translation isn't found in the current language
    translate.setDefaultLang('fr');
    const browserLang = translate.getBrowserLang();
    // the lang to use, if the lang isn't available, it will use the current loader to get them
    translate.use(browserLang ? browserLang : 'fr');
  }


  private initBaseUrl() {
    if (!Config.APP_URL.startsWith('http')) {
        const url = window.location.protocol + '//' + window.location.hostname + Config.APP_URL;
        Config.INIT_APP_URL(url);
    }
  }
}
