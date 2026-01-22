using P3tr0viCh.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TileExplorer.Properties;
using static TileExplorer.Database.Models;
using static TileExplorer.Enums;

namespace TileExplorer
{
    public partial class Main
    {
        private void TagAdd(TagModel tag)
        {
            if (!FrmTag.ShowDlg(this, tag)) return;

            Utils.Forms.GetFrmList(ChildFormType.TagList)?.ListItemChange(tag);
        }

        private async Task TagChangeAsync(TagModel tag)
        {
            if (!FrmTag.ShowDlg(this, tag)) return;

            await UpdateDataAsync(DataLoad.ObjectChange, tag);
        }

        public async Task TagChangedAsync(TagModel tag)
        {
            await UpdateDataAsync(DataLoad.ObjectChange, tag);

            Utils.Forms.GetFrmList(ChildFormType.TagList)?.SetSelected(tag);
        }

        private async Task TagDeleteAsync(List<TagModel> tags)
        {
            if (tags?.Count == 0) return;

            var firstTag = tags.FirstOrDefault();

            var text = firstTag.Text;

            var question = tags.Count == 1 ? Resources.QuestionTagDelete : Resources.QuestionTagListDelete;

            if (!Msg.Question(question, text, tags.Count - 1)) return;

            if (!await Database.Actions.TagDeleteAsync(tags)) return;

            foreach (var tag in tags)
            {
                await UpdateDataAsync(DataLoad.ObjectDelete, tag);
            }
        }
    }
}