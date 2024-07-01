import './App.css'
import { SigninForm } from './components/SigninForm/SigninForm';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { UserInfo } from './components/UserInfo/UserInfo';
import { Header } from './components/Header/Header';

function App() {
  const queryClient = new QueryClient();

  return <>
  <Header/>
  <QueryClientProvider client={queryClient}>
    <SigninForm />
    <br/>
    <UserInfo />
  </QueryClientProvider>
  </>;
}

export default App
