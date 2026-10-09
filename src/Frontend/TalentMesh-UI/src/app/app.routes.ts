import { Routes } from '@angular/router';
import { MainLayout } from './core/layout/main-layout/main-layout';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    //public routes
    {
        path: '',
        loadComponent: () =>
            import('./features/home/home/home')
                .then(m => m.Home)
    },

    {
        path: 'login',
        loadComponent: () =>
            import('./features/auth/login/login.component')
                .then(m => m.LoginComponent)
    },

    {
        path: 'signup',
        loadComponent: () =>
            import('./features/auth/signup/signup/signup')
                .then(m => m.Signup)
    },

    //public routes 
    {
        path: '',
        component : MainLayout,
        canActivate : [authGuard],
        children: [
            {
                path: 'dashboard',
                loadComponent: () =>
                    import('./features/dashboard/dashboard.component')
                        .then(m => m.DashboardComponent)
            },
            {
                path: 'projects',
                loadComponent: () =>
                    import('./features/projects/projects.component')
                        .then(m => m.ProjectComponent)
            },
            {
                path: 'projects/create',
                loadComponent: () =>
                    import('./features/create-project/create-project')
                        .then(m => m.CreateProject)
            },

            {
                path: 'projects/:id',
                loadComponent: () =>
                    import('./features/project-details/project-details')
                        .then(m => m.ProjectDetails)
            },
            {
                path: 'resource-pool',
                loadComponent: () =>
                    import('./features/resource-pool/resource-pool')
                        .then(m => m.Resourcepool)
                ,
            },
            {
                path: 'allocations/approvals',
                loadComponent: () =>
                    import('./features/approval-dashboard/approval-dashboard')
                        .then(m => m.ApprovalDashboard),
                // Apply your existing ResourceManager role guard here.
            }
        ]
    },
    {
        path: '**',
        redirectTo: ''
    },
];
