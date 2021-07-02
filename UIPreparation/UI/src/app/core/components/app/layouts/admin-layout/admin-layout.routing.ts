import { Routes } from '@angular/router';
import { GroupComponent } from 'app/core/components/admin/group/group.component';
import { LanguageComponent } from 'app/core/components/admin/language/language.component';
import { LogDtoComponent } from 'app/core/components/admin/log/logDto.component';
import { LoginComponent } from 'app/core/components/admin/login/login.component';
import { OperationClaimComponent } from 'app/core/components/admin/operationclaim/operationClaim.component';
import { TranslateComponent } from 'app/core/components/admin/translate/translate.component';
import { UserComponent } from 'app/core/components/admin/user/user.component';
import { LoginGuard } from 'app/core/guards/login-guard';
import { AlertComponent } from '../../alert/alert.component';
import { AlertActionComponent } from '../../alertAction/alertAction.component';
import { AlertActionLogComponent } from '../../alertActionLog/alertActionLog.component';
import { AlertActionUserComponent } from '../../alertActionUser/alertActionUser.component';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import { DeviceComponent } from '../../device/device.component';
import { DeviceSensorComponent } from '../../deviceSensor/deviceSensor.component';
import { LocationComponent } from '../../location/location.component';
import { ParameterComponent } from '../../parameter/parameter.component';
import { SensorComponent } from '../../sensor/sensor.component';
import { SensorLocationComponent } from '../../sensorLocation/sensorLocation.component';
import { SensorSettingComponent } from '../../sensorSetting/sensorSetting.component';
import { SensorValueComponent } from '../../sensorValue/sensorValue.component';
import { SettingComponent } from '../../setting/setting.component';





export const AdminLayoutRoutes: Routes = [

    { path: 'dashboard',      component: DashboardComponent,canActivate:[LoginGuard] }, 
    { path: 'user',           component: UserComponent, canActivate:[LoginGuard] },
    { path: 'group',          component: GroupComponent, canActivate:[LoginGuard] },
    { path: 'login',          component: LoginComponent },
    { path: 'language',       component: LanguageComponent,canActivate:[LoginGuard]},
    { path: 'translate',      component: TranslateComponent,canActivate:[LoginGuard]},
    { path: 'operationclaim', component: OperationClaimComponent,canActivate:[LoginGuard]},
    { path: 'log',            component: LogDtoComponent,canActivate:[LoginGuard]},
    { path: 'alert',          component: AlertComponent,canActivate:[LoginGuard]},
    { path: 'alert-action',   component: AlertActionComponent,canActivate:[LoginGuard]},
    { path: 'location',       component: LocationComponent,canActivate:[LoginGuard]},
    { path: 'parameter',      component: ParameterComponent,canActivate:[LoginGuard]},
    { path: 'sensor',         component: SensorComponent,canActivate:[LoginGuard]},
    { path: 'sensor-location',component: SensorLocationComponent,canActivate:[LoginGuard]},
    { path: 'sensor-setting', component: SensorSettingComponent,canActivate:[LoginGuard]},
    { path: 'sensor-value',   component: SensorValueComponent,canActivate:[LoginGuard]},
    { path: 'setting',        component: SettingComponent,canActivate:[LoginGuard]},
    { path: 'alert-action-user',        component: AlertActionUserComponent,canActivate:[LoginGuard]},
    { path: 'alert-action-log',        component: AlertActionLogComponent,canActivate:[LoginGuard]},
    { path: 'device',        component: DeviceComponent,canActivate:[LoginGuard]},
    { path: 'device-sensor',        component: DeviceSensorComponent,canActivate:[LoginGuard]},
        
];
