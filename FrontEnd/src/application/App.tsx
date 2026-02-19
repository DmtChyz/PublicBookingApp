import { lazy, Suspense } from 'react';
import Header from '../components/Header/Header';
import Footer from '../components/Footer/Footer';
import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import LoadingScreen from '../components/LoadingScreen/LoadingScreen';
import ProtectedRoute from '../components/ProtectedRoute/ProtectedRoute';
import RegisterForm from '../pages/RegisterForm/RegisterForm';
import { AuthProvider } from '../context/AuthContext/AuthContext';
import { NotificationProvider } from '../context/NotificationContext/NotificationContext';
import EventDetailsPage from '../pages/EventDetailsPage/EventDetailsPage';

const AboutUsPage = lazy(() => import('../pages/AboutUs/AboutUsPage'));
const EventList = lazy(() => import('../pages/EventList/EventList'));
const RightsPage = lazy(() => import('../pages/Rights/RightsPage'));
const ContactsPage = lazy(() => import('../pages/Contacts/ContactsPage'));
const SupportPage = lazy(() => import('../pages/Support/Support'));
const ForContributorsPage = lazy(() => import('../pages/ForContributors/ForContributors'));
const LoginForm = lazy(() => import('../pages/LoginForm/LoginForm'));
const UserProfile = lazy(() => import('../pages/UserProfile/UserProfile'));
const ResetPassword = lazy(() => import('../pages/ResetPassword/ResetPasswordPage'));
const ForgotPassword = lazy(() => import('../pages/ForgotPasswordPage/ForgotPasswordPage'));
const CreateEventPage = lazy(() => import('../pages/CreateEventPage/CreateEventPage'));
const EditEventPage = lazy(() => import('../pages/EditEventPage/EditEventPage'));

function App() {
  return (
    <AuthProvider>
      <NotificationProvider>
        <BrowserRouter>
          <div className="appContainer">
            <Header />
            <div className={"mainWrapper"}>
              <main className="mainContent">
                <Suspense fallback={<LoadingScreen/>}>
                  <Routes>
                    <Route path='/events' element={<EventList/>} />
                    <Route path='/events/:eventId' element={<EventDetailsPage/>}/>
                    <Route path='/rights' element={<RightsPage/>}/>
                    <Route path='/support' element={<SupportPage/>}/>
                    <Route path='/for-contributors' element ={<ForContributorsPage/>}/>
                    <Route path='/contacts' element={<ContactsPage/>}/>
                    <Route path='/about' element={<AboutUsPage />} />
                    <Route path='/' element={<AboutUsPage />} />
                    <Route path='/login' element={<LoginForm/>} />
                    <Route path='/register' element={<RegisterForm/>}/>
                    <Route path='/reset-password' element={<ResetPassword/>}/>
                    <Route path='/forgot-password' element={<ForgotPassword/>}/>
                    
                    <Route element={<ProtectedRoute />}>
                      <Route path='/profile' element={<UserProfile/>}/>
                      <Route path='/createEvent' element={<CreateEventPage/>}/>
                      <Route path="/events/:id/edit" element={<EditEventPage/>}/>
                    </Route>
                  </Routes>
                </Suspense>
              </main>
            </div>
            <Footer />
          </div>
        </BrowserRouter>
      </NotificationProvider>
    </AuthProvider>
  );
}

export default App;