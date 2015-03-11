using BLL.Interface.Abstract;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using ToDoManager.Models;
using ToDoManager.Services;
using ToDoManager.Mapping;
using System.Linq;

namespace ToDoManager.Controllers
{
	/// <summary>
	/// The to do controller.
	/// </summary>
	public class ToDoController : Controller
	{
		private static IAuthenticationService _authenticationService;
		private IToDoItemService _todoService;
        private IToDoItemServiceCache cache;


		#region Constructors and Destructors

		public ToDoController()
		{
			var userService = new UserService();

			_authenticationService = userService;

            this._todoService = (IToDoItemService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IToDoItemService));
            this.cache = (IToDoItemServiceCache)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IToDoItemServiceCache));
			_authenticationService.Login(null);
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// The all.
		/// </summary>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		public async Task<ActionResult> All()
		{
            var result = await _todoService.GetAll();
            return this.View(result.Select(x => x.ToUi()));
		}

		/// <summary>
		/// The create.
		/// </summary>
		/// <returns>
		/// The <see cref="ActionResult"/>.
		/// </returns>
		public ActionResult Create()
		{	
			return this.View();
		}

		/// <summary>
		/// The create.
		/// </summary>
		/// <param name="todo">
		/// The todo.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		[HttpPost]
		public async Task<ActionResult> Create([Bind(Include = "Id,Description,IsDone")] ToDoModel todo)
		{
			Thread.Sleep(5000); // ui works fastly with it too
            if (this.ModelState.IsValid)
            {
                await _todoService.Create(todo.ToBll());
                return this.Json(todo);
            }
            return this.Json(null);
		}

		/// <summary>
		/// The delete.
		/// </summary>
		/// <param name="id">
		/// The id.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

            var result = await _todoService.GetById(id.Value);
            ToDoModel todo = result.ToUi();

			if (todo == null)
			{
				return this.HttpNotFound();
			}

			return this.View(todo);
		}

		/// <summary>
		/// The delete confirmed.
		/// </summary>
		/// <param name="id">
		/// The id.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		[HttpPost]
		[ActionName("Delete")]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
            var result = await _todoService.GetById(id); ;

            ToDoModel todo = result.ToUi();

			if (todo == null)
			{
				return this.Json(null);
			}

			await _todoService.RemoveById(id);
            return this.Json(todo);
		}

		/// <summary>
		/// The details.
		/// </summary>
		/// <param name="id">
		/// The id.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

            var result = await _todoService.GetById(id.Value);
            ToDoModel todo = result.ToUi();

			if (todo == null)
			{
				return this.HttpNotFound();
			}

			return this.View(todo);
		}

		/// <summary>
		/// The edit.
		/// </summary>
		/// <param name="id">
		/// The id.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

            var result = await _todoService.GetById(id.Value);
            ToDoModel todo = result.ToUi();

			if (todo == null)
			{
				return this.HttpNotFound();
			}

			return this.View(todo);
		}

		/// <summary>
		/// The edit.
		/// </summary>
		/// <param name="todo">
		/// The todo.
		/// </param>
		/// <returns>
		/// The <see cref="Task"/>.
		/// </returns>
		[HttpPost]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Description,IsDone")] ToDoModel todo)
		{
			if (this.ModelState.IsValid)
            {
                await _todoService.Update(todo.ToBll());
                return this.Json(todo);
            }
			return this.Json(null);
		}

		/// <summary>
		/// The index.
		/// </summary>
		/// <returns>
		/// The <see cref="ActionResult"/>.
		/// </returns>
		public async Task<ActionResult> Index()
		{
            var result = await _todoService.GetAll();

			return this.View(result.Select(x => x.ToUi()));
		}

		#endregion
	}
}