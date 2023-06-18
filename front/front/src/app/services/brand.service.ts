import { Injectable } from '@angular/core';
import {PSCommonService} from './ps-common.service';
import {HttpClient} from '@angular/common/http';
import {Observable, of} from 'rxjs';
import {map} from 'rxjs/operators';
import { NGXLogger } from 'ngx-logger';
import {Config} from '../config';
import {Brand} from '../model/brand';

@Injectable({
  providedIn: 'root'
})
export class BrandService extends PSCommonService {

  constructor(private http: HttpClient, protected override readonly logger: NGXLogger ) {
    super(logger);
  }
   /**
     * Get the list of available data
     */
  public findAll(): Observable<Brand[]> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.brand}`;
    return this.http.get<Array<Brand>>(url).pipe(map(resu => {
        const list: Brand[] = [];

        for (const u of resu) {
            list.push(new Brand(u));
        }
        return list;
    }));
  }
}
