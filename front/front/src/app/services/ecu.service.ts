import { Injectable } from '@angular/core';
import {PSCommonService} from './ps-common.service';
import {HttpClient} from '@angular/common/http';
import {Observable, of} from 'rxjs';
import {map} from 'rxjs/operators';
import { NGXLogger } from 'ngx-logger';
import {Config} from '../config';
import {Ecu} from '../model/ecu';

@Injectable({
  providedIn: 'root'
})
export class EcuService extends PSCommonService {

  constructor(private http: HttpClient, protected override readonly logger: NGXLogger ) {
    super(logger);
  }
   /**
     * Get the list of available data
     */
  public findByBrand(brandCode: string): Observable<Ecu[]> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.ecu}/${brandCode}`;
    return this.http.get<Array<Ecu>>(url).pipe(map(resu => {
        const list: Ecu[] = [];

        for (const u of resu) {
            list.push(new Ecu(u));
        }
        return list;
    }));
  }

     /**
     * Get the list of available data
     */
     public findByBrandAndFuel(brandCode: string, fuel: string): Observable<Ecu[]> {
      const url = `${Config.APP_URL}${Config.API_ROUTES.ecu}/${brandCode}/${fuel}`;
      return this.http.get<Array<Ecu>>(url).pipe(map(resu => {
          const list: Ecu[] = [];
  
          for (const u of resu) {
              list.push(new Ecu(u));
          }
          return list;
      }));
    }

 /**
  * delete
  */
  public delete(brandCode: string, code: string): Observable<boolean> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.ecu}/${brandCode}/${code}`;
  
    return this.http.delete<boolean>(url).pipe(map(p => {
      return true;
    }));
  }

  
 /**
  * Add
  */
 public create(tenant: Ecu): Observable<Ecu> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.ecu}`;

    return this.http.post(url, tenant).pipe(map(p => {
      return new Ecu(p);
    }));
  }


  /**
  * Update
  */
  public set(tenant: Ecu): Observable<Ecu> {
    const url = `${Config.APP_URL}${Config.API_ROUTES.ecu}`;

    return this.http.put(url, tenant).pipe(map(p => {
      return new Ecu(p);
    }));
  }

}
