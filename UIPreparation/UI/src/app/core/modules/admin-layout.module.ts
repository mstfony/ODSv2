import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminLayoutRoutes } from '../components/app/layouts/admin-layout/admin-layout.routing';
import { DashboardComponent } from '../components/app/dashboard/dashboard.component';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatRippleModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginatorModule } from '@angular/material/paginator';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { LoginComponent } from 'app/core/components/admin/login/login.component';
import { GroupComponent } from 'app/core/components/admin/group/group.component';
import { UserComponent } from 'app/core/components/admin/user/user.component';
import { TranslateLoader, TranslateModule, TranslatePipe } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslationService } from 'app/core/services/translation.service';
import { LanguageComponent } from '../components/admin/language/language.component';
import { TranslateComponent } from '../components/admin/translate/translate.component';
import { OperationClaimComponent } from '../components/admin/operationclaim/operationClaim.component';
import { LogDtoComponent } from '../components/admin/log/logDto.component';
import { MatSortModule } from '@angular/material/sort';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { AlertComponent } from '../components/app/alert/alert.component';
import { AlertActionComponent } from '../components/app/alertAction/alertAction.component';
import { LocationComponent } from '../components/app/location/location.component';
import { ParameterComponent } from '../components/app/parameter/parameter.component';
import { SensorComponent } from '../components/app/sensor/sensor.component';
import { SensorLocationComponent } from '../components/app/sensorLocation/sensorLocation.component';
import { SensorSettingComponent } from '../components/app/sensorSetting/sensorSetting.component';
import { SensorValueComponent } from '../components/app/sensorValue/sensorValue.component';
import { SettingComponent } from '../components/app/setting/setting.component';
import { AlertActionUserComponent } from '../components/app/alertActionUser/alertActionUser.component';
import { AlertActionLogComponent } from '../components/app/alertActionLog/alertActionLog.component';
import { DeviceComponent } from '../components/app/device/device.component';
import { DeviceSensorComponent } from '../components/app/deviceSensor/deviceSensor.component';

// export function layoutHttpLoaderFactory(http: HttpClient) {
// 
//   return new TranslateHttpLoader(http,'../../../../../../assets/i18n/','.json');
// }

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(AdminLayoutRoutes),
        FormsModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatRippleModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatTooltipModule,
        MatTableModule,
        MatPaginatorModule,
        MatSortModule,
        MatCheckboxModule,
        NgbModule,
        NgMultiSelectDropDownModule,
        SweetAlert2Module,
        TranslateModule.forChild({
            loader: {
                provide: TranslateLoader,
                //useFactory:layoutHttpLoaderFactory,
                useClass: TranslationService,
                deps: [HttpClient]
            }
        })
    ],
    declarations: [
        DashboardComponent,
        UserComponent,
        LoginComponent,
        GroupComponent,
        LanguageComponent,
        TranslateComponent,
        OperationClaimComponent,
        LogDtoComponent,
        AlertComponent,
        AlertActionComponent,
        LocationComponent,
        ParameterComponent,
        SensorComponent,
        SensorLocationComponent,
        SensorSettingComponent,
        SensorValueComponent,
        SettingComponent,
        AlertActionUserComponent,
        AlertActionLogComponent,
        DeviceComponent,
        DeviceSensorComponent


    ]
})

export class AdminLayoutModule { }
