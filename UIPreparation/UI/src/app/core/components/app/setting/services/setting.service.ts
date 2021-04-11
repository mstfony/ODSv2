import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Setting } from '../models/Setting';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class SettingService {

  constructor(private httpClient: HttpClient) { }


  getSettingList(): Observable<Setting[]> {

    return this.httpClient.get<Setting[]>(environment.getApiUrl + '/settings/getall')
  }

  getSettingById(id: number): Observable<Setting> {
    return this.httpClient.get<Setting>(environment.getApiUrl + '/settings/getbyid?id='+id)
  }

  addSetting(setting: Setting): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/settings/', setting, { responseType: 'text' });
  }

  updateSetting(setting: Setting): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/settings/', setting, { responseType: 'text' });

  }

  deleteSetting(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/settings/', { body: { id: id } });
  }


}