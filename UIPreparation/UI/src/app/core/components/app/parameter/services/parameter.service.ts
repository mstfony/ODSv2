import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Parameter } from '../models/Parameter';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ParameterService {

  constructor(private httpClient: HttpClient) { }


  getParameterList(): Observable<Parameter[]> {

    return this.httpClient.get<Parameter[]>(environment.getApiUrl + '/parameters/getall')
  }

  getParameterById(id: number): Observable<Parameter> {
    return this.httpClient.get<Parameter>(environment.getApiUrl + '/parameters/getbyid?id='+id)
  }

  addParameter(parameter: Parameter): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/parameters/', parameter, { responseType: 'text' });
  }

  updateParameter(parameter: Parameter): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/parameters/', parameter, { responseType: 'text' });

  }

  deleteParameter(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/parameters/', { body: { id: id } });
  }


}