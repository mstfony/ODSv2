import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AlertAction } from '../models/AlertAction';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AlertActionService {

  constructor(private httpClient: HttpClient) { }


  getAlertActionList(): Observable<AlertAction[]> {

    return this.httpClient.get<AlertAction[]>(environment.getApiUrl + '/alertActions/getall')
  }

  getAlertActionById(id: number): Observable<AlertAction> {
    return this.httpClient.get<AlertAction>(environment.getApiUrl + '/alertActions/getbyid?id='+id)
  }

  addAlertAction(alertAction: AlertAction): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/alertActions/', alertAction, { responseType: 'text' });
  }

  updateAlertAction(alertAction: AlertAction): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/alertActions/', alertAction, { responseType: 'text' });

  }

  deleteAlertAction(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/alertActions/', { body: { id: id } });
  }


}