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

  /**
   * Get the list of available data
   */
  public findByCode(code: string): Observable<Brand|null> {
      const url = `${Config.APP_URL}${Config.API_ROUTES.brand}/${code}`;
      return this.http.get<Brand>(url).pipe(map(b => {
          let resu:Brand|null = null;
          if (b) {
            resu = new Brand(b);
          }
          return resu;
      }));
  }
  
 /**
  * Add
  */
  public create(tenant: Brand): Observable<Brand> {
      const url = `${Config.APP_URL}${Config.API_ROUTES.brand}`;
  
      return this.http.post(url, tenant).pipe(map(p => {
        return new Brand(p);
      }));
  }
  

 /**
  * Update
  */
  public set(tenant: Brand): Observable<Brand> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.brand}`;

    return this.http.put(url, tenant).pipe(map(p => {
      return new Brand(p);
    }));
  }

 /**
  * delete
  */
   public delete(code: string): Observable<boolean> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.brand}/${code}`;
  
    return this.http.delete<boolean>(url).pipe(map(p => {
      return true;
    }));
  }

}
