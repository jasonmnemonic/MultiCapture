#include "ProjectView.h"
#include <qtimer.h>
#include "Lib/json.hpp"
#include "Project.h"
#include <QInputDialog>
#include <QMenu>
#include "parameterBuilder.h"
#include <qdir.h>
#include <QLineEdit>

ProjectView::ProjectView(QPushButton* refresh, QPushButton* transfer, QTableView* table, ScannerInteraction* connector)
{
	//assign external members
	this->refresh = refresh;
	this->transfer = transfer;
	this->table = table;

	this->connector = connector;

	dataModel = new ProjectTableView();
	table->setModel(dataModel);

	table->resizeColumnsToContents();
	table->setContextMenuPolicy(Qt::CustomContextMenu);
	produceContextMenu();

	connect(refresh, &QPushButton::clicked, this, &ProjectView::refreshProjects);
	connect(table, &QTableView::customContextMenuRequested, this, &ProjectView::createCustomMenu);
}

ProjectView::~ProjectView()
{
	delete refresh;
	delete transfer;
	delete table;

	//delete timer;
}

void ProjectView::respondToScanner(ScannerCommands command, QByteArray data)
{
	switch (command)
	{
	case ScannerCommands::getLoadedProjects:
		processProjects(data);
		break;
	default:
		return;
	}
}

void ProjectView::refreshProjects()
{
	connector->requestScanner(ScannerCommands::getLoadedProjects, "", this);
}

void ProjectView::createCustomMenu(const QPoint& pos)
{
	//check what cell is requested
	QModelIndex index = table->indexAt(pos);
	if (!dataModel->canChangeName(index)) return;

	//show context menu for changing the project name
	QPoint location = table->mapToGlobal(pos);
	contextMenuIndex = new QModelIndex(index);
	nameChange->exec(location);
}

void ProjectView::changeProjectName()
{
	bool ok;
	int project = dataModel->getProjectId(*contextMenuIndex);

	QString text = QInputDialog::getText(table, tr("New Project Name"),
		tr("New Name"), QLineEdit::Normal, QString::number(project), &ok);

	//check if user canceled
	if (!ok) return;

	connector->requestScanner(ScannerCommands::setProjectNiceName,
		parameterBuilder().addParam("id", QString::number(project))->
		addParam("name", text)->toString(), this);
	refreshProjects();
}

void ProjectView::processProjects(QByteArray data) const
{
	nlohmann::json result = nlohmann::json::parse(data.toStdString().c_str());

	dataModel->clearData();

	//collect all the data from the response
	for (int i = 0; i < result.size(); ++i)
	{
		std::string normal = result[i].dump();
		nlohmann::json jsonProject = nlohmann::json::parse(normal.c_str());

		project instance = project();

		instance.id = jsonProject.at("Id").get<int>();
		instance.name = jsonProject.at("Name").get<std::string>();
		instance.imageCount = jsonProject.at("ImageCount").get<int>();
		instance.imagesAvaliable = jsonProject.at("SavedCount").get<int>();

		dataModel->addItem(instance);
	}

	dataModel->updateTable();

	table->resizeColumnsToContents();
}

void ProjectView::produceContextMenu()
{
	nameChange = new QMenu();

	QAction* name = nameChange->addAction("Change Name");
	name->setStatusTip("Change the name of the project");
	connect(name, &QAction::triggered, this, &ProjectView::changeProjectName);
}
