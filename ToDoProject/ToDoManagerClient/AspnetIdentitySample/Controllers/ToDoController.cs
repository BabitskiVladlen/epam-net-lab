using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

using ToDoManager.Models;
using ToDoManager.Services;

namespace ToDoManager.Controllers
{
	/// <summary>
	/// The to do controller.
	/// </summary>
	public class ToDoController : Controller
	{
		private static IAuthenticationService _authenticationService;
		private static IToDoItemService _todoService;

		#region Constructors and Destructors

		static ToDoController()
		{
			var userService = new UserService();

			_authenticationService = userService;
			_todoService = new ToDoItemService(userService);

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
			return View(await _todoService.GetAll());
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
			Thread.Sleep(5000);
			if (this.ModelState.IsValid)
			{
				await _todoService.Create(todo);
				return this.RedirectToAction("Index");
			}

			return this.View(todo);
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

			ToDoModel todo = await _todoService.GetById(id.Value);
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
			ToDoModel todo = await _todoService.GetById(id);
			if (todo == null)
			{
				return this.HttpNotFound();
			}

			await _todoService.RemoveById(id);
			return this.RedirectToAction("Index");
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

			ToDoModel todo = await _todoService.GetById(id.Value);
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
			ToDoModel todo = await _todoService.GetById(id.Value);
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
				await _todoService.Update(todo);
				return this.RedirectToAction("Index");
			}

			return this.View(todo);
		}

		/// <summary>
		/// The index.
		/// </summary>
		/// <returns>
		/// The <see cref="ActionResult"/>.
		/// </returns>
		public async Task<ActionResult> Index()
		{
			return View(await _todoService.GetAll());
		}

		#endregion
	}
}