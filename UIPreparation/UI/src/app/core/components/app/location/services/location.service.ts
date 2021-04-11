import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Location } from '../models/Location';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class LocationService {

  constructor(private httpClient: HttpClient) { }


  getLocationList(): Observable<Location[]> {

    return this.httpClient.get<Location[]>(environment.getApiUrl + '/locations/getall')
  }

  getLocationById(id: number): Observable<Location> {
    return this.httpClient.get<Location>(environment.getApiUrl + '/locations/getbyid?id='+id)
  }

  addLocation(location: Location): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/locations/', location, { responseType: 'text' });
  }

  updateLocation(location: Location): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/locations/', location, { responseType: 'text' });

  }

  deleteLocation(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/locations/', { body: { id: id } });
  }


}