import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { HomeComponent } from './home/home.component';
import { Routes } from '@angular/router';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailedComponent } from './members/member-detailed/member-detailed.component';
import { MemberDetailResolver } from './_routeResolvers/member-detail.resolver';
import { MemberListResolver } from './_routeResolvers/member-list.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        canActivate: [AuthGuard],
        runGuardsAndResolvers: 'always',
        children: [
            { path: 'messages', component: MessagesComponent },
            { path: 'members/:id', component: MemberDetailedComponent , resolve: {userRouteResolver: MemberDetailResolver}},
            { path: 'members', component: MemberListComponent , resolve: {usersRouteResolver: MemberListResolver}},
            { path: 'lists', component: ListsComponent }
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
