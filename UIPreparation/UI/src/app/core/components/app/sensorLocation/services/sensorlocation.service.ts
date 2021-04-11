import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SensorLocation } from '../models/SensorLocation';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class SensorLocationService {

  constructor(private httpClient: HttpClient) { }


  getSensorLocationList(): Observable<SensorLocation[]> {

    return this.httpClient.get<SensorLocation[]>(environment.getApiUrl + '/sensorLocations/getall')
  }

  getSensorLocationById(id: number): Observable<SensorLocation> {
    return this.httpClient.get<SensorLocation>(environment.getApiUrl + '/sensorLocations/getbyid?id='+id)
  }

  addSensorLocation(sensorLocation: SensorLocation): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/sensorLocations/', sensorLocation, { responseType: 'text' });
  }

  updateSensorLocation(sensorLocation: SensorLocation): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/sensorLocations/', sensorLocation, { responseType: 'text' });

  }

  deleteSensorLocation(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/sensorLocations/', { body: { id: id } });
  }


}