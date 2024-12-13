export default function({store, redirect}) {
  if (!store.getters['UserStore/jwtToken']) return redirect('/login?denied=true');
}
