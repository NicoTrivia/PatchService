import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {RequestPatchComponent} from './nav/request-patch/request-patch.component';

const routes: Routes = [
  { path: '', component: RequestPatchComponent   },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
