import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: 'login',
        loadComponent: () =>
            import('./features/auth/login/login.component')
                .then(m => m.LoginComponent)
    },

    {
        path: 'dashboard',
        loadComponent: () =>
            import('./features/dashboard/dashboard.component')
                .then(m => m.DashboardComponent)
    },
    {
        path : 'resource-pool',
        loadComponent : () => 
            import('./features/resource-pool/resource-pool')
            .then(m => m.Resourcepool)
        ,
    },

    {
        path: '',
        loadComponent: () =>
            import('./app')
                .then(m => m.App)
    },

    // {
    //     path: '',
    //     redirectTo: 'login',
    //     pathMatch: 'full'
    // }
];
