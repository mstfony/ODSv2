import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Parameter } from './models/Parameter';
import { ParameterService } from './services/Parameter.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-parameter',
	templateUrl: './parameter.component.html',
	styleUrls: ['./parameter.component.scss']
})
export class ParameterComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','name', 'update','delete'];

	parameterList:Parameter[];
	parameter:Parameter=new Parameter();

	parameterAddForm: FormGroup;


	parameterId:number;

	constructor(private parameterService:ParameterService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getParameterList();
    }

	ngOnInit() {

		this.createParameterAddForm();
	}


	getParameterList() {
		this.parameterService.getParameterList().subscribe(data => {
			this.parameterList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.parameterAddForm.valid) {
			this.parameter = Object.assign({}, this.parameterAddForm.value)

			if (this.parameter.id == 0)
				this.addParameter();
			else
				this.updateParameter();
		}

	}

	addParameter(){

		this.parameterService.addParameter(this.parameter).subscribe(data => {
			this.getParameterList();
			this.parameter = new Parameter();
			jQuery('#parameter').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.parameterAddForm);

		})

	}

	updateParameter(){

		this.parameterService.updateParameter(this.parameter).subscribe(data => {

			var index=this.parameterList.findIndex(x=>x.id==this.parameter.id);
			this.parameterList[index]=this.parameter;
			this.dataSource = new MatTableDataSource(this.parameterList);
            this.configDataTable();
			this.parameter = new Parameter();
			jQuery('#parameter').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.parameterAddForm);

		})

	}

	createParameterAddForm() {
		this.parameterAddForm = this.formBuilder.group({		
			id : [0],
name : ["", Validators.required]
		})
	}

	deleteParameter(parameterId:number){
		this.parameterService.deleteParameter(parameterId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.parameterList=this.parameterList.filter(x=> x.id!=parameterId);
			this.dataSource = new MatTableDataSource(this.parameterList);
			this.configDataTable();
		})
	}

	getParameterById(parameterId:number){
		this.clearFormGroup(this.parameterAddForm);
		this.parameterService.getParameterById(parameterId).subscribe(data=>{
			this.parameter=data;
			this.parameterAddForm.patchValue(data);
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
