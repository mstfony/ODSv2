import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Alert } from '../models/Alert';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AlertService {

  constructor(private httpClient: HttpClient) { }


  getAlertList(): Observable<Alert[]> {

    return this.httpClient.get<Alert[]>(environment.getApiUrl + '/alerts/getall')
  }

  getAlertById(id: number): Observable<Alert> {
    return this.httpClient.get<Alert>(environment.getApiUrl + '/alerts/getbyid?id='+id)
  }

  addAlert(alert: Alert): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/alerts/', alert, { responseType: 'text' });
  }

  updateAlert(alert: Alert): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/alerts/', alert, { responseType: 'text' });

  }

  deleteAlert(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/alerts/', { body: { id: id } });
  }


}