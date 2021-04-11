import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Location } from './models/Location';
import { LocationService } from './services/Location.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-location',
	templateUrl: './location.component.html',
	styleUrls: ['./location.component.scss']
})
export class LocationComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','name', 'update','delete'];

	locationList:Location[];
	location:Location=new Location();

	locationAddForm: FormGroup;


	locationId:number;

	constructor(private locationService:LocationService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getLocationList();
    }

	ngOnInit() {

		this.createLocationAddForm();
	}


	getLocationList() {
		this.locationService.getLocationList().subscribe(data => {
			this.locationList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.locationAddForm.valid) {
			this.location = Object.assign({}, this.locationAddForm.value)

			if (this.location.id == 0)
				this.addLocation();
			else
				this.updateLocation();
		}

	}

	addLocation(){

		this.locationService.addLocation(this.location).subscribe(data => {
			this.getLocationList();
			this.location = new Location();
			jQuery('#location').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.locationAddForm);

		})

	}

	updateLocation(){

		this.locationService.updateLocation(this.location).subscribe(data => {

			var index=this.locationList.findIndex(x=>x.id==this.location.id);
			this.locationList[index]=this.location;
			this.dataSource = new MatTableDataSource(this.locationList);
            this.configDataTable();
			this.location = new Location();
			jQuery('#location').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.locationAddForm);

		})

	}

	createLocationAddForm() {
		this.locationAddForm = this.formBuilder.group({		
			id : [0],
name : ["", Validators.required]
		})
	}

	deleteLocation(locationId:number){
		this.locationService.deleteLocation(locationId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.locationList=this.locationList.filter(x=> x.id!=locationId);
			this.dataSource = new MatTableDataSource(this.locationList);
			this.configDataTable();
		})
	}

	getLocationById(locationId:number){
		this.clearFormGroup(this.locationAddForm);
		this.locationService.getLocationById(locationId).subscribe(data=>{
			this.location=data;
			this.locationAddForm.patchValue(data);
		})
	}


	clearFormGroup(group: FormGroup) {

		group.markAsUntouched();
		group.reset();

		Object.keys(group.controls).forEach(key => {
			group.get(key).setErrors(null);
			if (key == 'id')
				group.get(key).setValue(0);
		});
	}

	checkClaim(claim:string):boolean{
		return this.authService.claimGuard(claim)
	}

	configDataTable(): void {
		this.dataSource.paginator = this.paginator;
		this.dataSource.sort = this.sort;
	}

	applyFilter(event: Event) {
		const filterValue = (event.target as HTMLInputElement).value;
		this.dataSource.filter = filterValue.trim().toLowerCase();

		if (this.dataSource.paginator) {
			this.dataSource.paginator.firstPage();
		}
	}

  }
