import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Sensor } from '../models/Sensor';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class SensorService {

  constructor(private httpClient: HttpClient) { }


  getSensorList(): Observable<Sensor[]> {

    return this.httpClient.get<Sensor[]>(environment.getApiUrl + '/sensors/getall')
  }

  getSensorById(id: number): Observable<Sensor> {
    return this.httpClient.get<Sensor>(environment.getApiUrl + '/sensors/getbyid?id='+id)
  }

  addSensor(sensor: Sensor): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/sensors/', sensor, { responseType: 'text' });
  }

  updateSensor(sensor: Sensor): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/sensors/', sensor, { responseType: 'text' });

  }

  deleteSensor(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/sensors/', { body: { id: id } });
  }


}